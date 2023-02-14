using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    /// <summary>
    /// Groups group schedule pair data into pair models, handles data mismatches (TODO: extract to other class).
    /// </summary>
    public class GroupSchedulePairDataGroupper
    {
        private readonly ILogger logger;
        private readonly TeachersScheduleCache teachersSchedulesCache;

        public GroupSchedulePairDataGroupper(ILogger logger,
            TeachersScheduleCache teachersSchedules)
        {
            this.logger = logger;
            this.teachersSchedulesCache = teachersSchedules;
        }

        public IEnumerable<RozKpiApiGroupPair> GroupPairData(RozKpiApiGroupPairData data)
        {
            if (IsMatchingPairData(data))
            {
                // no fixing needed, create and return
                return CreatePairsFromData(data);
            }

            logger.Warning("Data mismatch while parsing schedule cell: {subjectsCount} subjects, {teachersCount} teachers, {pairInfoCount} pair infos", data.SubjectNames.Count(), data.Teachers.Count(), data.PairInfos.Count());

            if (IsSinglePairWithSeveralTeachers(data))
            {
                logger.Information("Schedule cell has single pair with several teachers, copying all teachers to first pair");
                CopyAllTeachersToFirstPair(ref data);
            }

            if (AreSomePairsWithSeveralTeachers(data))
            {
                logger.Information("Schedule cell has some pairs with several teachers, looking into teacher schedule to group teachers by pair");
                var fixedData = FixPairsWithSeveralTeachers(data);
                data = fixedData;
            }

            if (AreMultiplePairsWithSinglePairInfo(data))
            {
                logger.Information("Schedule cell has multiple pairs with single pair info, copying pair info to all pairs");
                CopySinglePairInfoToAllPairs(ref data);
            }

            if (IsNonMatchingPairInfoAndPairsCount(data))
            {
                logger.Information("Schedule cell has less pair infos than pairs, looking into teacher schedule to get pair info for each pair");
                var fixedData = FixNonMatchingPairInfoAndPairsCount(data);
                data = fixedData;
            }

            if (IsSinglePairWithSeveralRooms(data))
            {
                logger.Information("Schedule cell has single pair with multiple pair infos, copying all rooms from pair infos to first pair");
                CopyAllRoomsToFirstPair(ref data);
            }

            if (AreSomePairsWithSeveralRooms(data))
            {
                logger.Information("Schedule cell some pairs with several rooms, looking into teacher schedule to get rooms for each pair");
                var fixedData = FixSomePairsWithSeveralRooms(data);
                data = fixedData;
            }

            if (AreSeveralPairsWithOneTeacher(data))
            {
                logger.Information("Schedule cell has multiple pairs with single teacher, copying teacher to all pairs");
                CopyFirstTeacherToAllPairs(ref data);
            }

            if (AreMorePairsThanTeachers(data))
            {
                logger.Information("Schedule cell has more pairs than teachers, looking into teacher schedules to assign them to correct pairs");
                var fixedData = FixMorePairsThanTeachers(data);
                data = fixedData;
            }

            return CreatePairsFromData(data);
        }

        private IEnumerable<RozKpiApiTeacherSchedule> GetTeacherSchedules(RozKpiApiGroupPairData data)
        {
            var teacherSchedulesTasks = data.Teachers
                .SelectMany(t => t)
                .Select(t => t.ScheduleId)
                .Select(id => teachersSchedulesCache.GetTeacherSchedule(id));
            var teacherSchedules = Task.WhenAll(teacherSchedulesTasks).Result;
            return teacherSchedules;
        }

        private RozKpiApiGroupPairData FixMorePairsThanTeachers(RozKpiApiGroupPairData data)
        {
            // look at teacher's schedule, distribute teachers by subject names
            // and leave some lessons with empty teacher lists
            var teacherSchedules = GetTeacherSchedules(data);

            var teacherPairs = teacherSchedules.Select(s => s.GetPair(data.Identifier)).ToList();

            var newTeachers = new RozKpiApiTeacher[data.SubjectNames.Count()][];
            for (int i = 0; i < data.SubjectNames.Count(); i++)
            {
                var subjectName = data.SubjectNames.ElementAt(i);
                var matchingTeacherIndex = teacherPairs.FindIndex(p => p.Subject.SubjectName == subjectName);
                if (matchingTeacherIndex != -1)
                {
                    newTeachers[i] = data.Teachers[matchingTeacherIndex];
                }
                else
                {
                    newTeachers[i] = Array.Empty<RozKpiApiTeacher>();
                }
            }
            data.Teachers = newTeachers;
            return data;
        }

        private void CopyFirstTeacherToAllPairs(ref RozKpiApiGroupPairData data)
        {
            var firstTeacher = data.Teachers[0][0];
            var pairsCount = data.SubjectNames.Count();
            var newTeachersArray = Enumerable.Repeat(new[] { firstTeacher }, pairsCount).ToArray();

            data.Teachers = newTeachersArray;
        }

        private RozKpiApiGroupPairData FixSomePairsWithSeveralRooms(RozKpiApiGroupPairData data)
        {
            // go through teachers schedules and get rooms for each lesson
            throw new NotImplementedException();
        }

        private void CopyAllRoomsToFirstPair(ref RozKpiApiGroupPairData data)
        {
            var rooms = data.PairInfos.SelectMany(pi => pi.Rooms);
            var newPairInfos = data.PairInfos.ToList();
            newPairInfos.ForEach(pi => pi.Rooms = Array.Empty<string>());
            newPairInfos[0].Rooms = rooms.ToList();

            data.PairInfos = newPairInfos;
        }

        private RozKpiApiGroupPairData FixNonMatchingPairInfoAndPairsCount(RozKpiApiGroupPairData data)
        {
            var teacherSchedules = GetTeacherSchedules(data);

            var teacherPairInfos = teacherSchedules
                .Select(s => s.GetPair(data.Identifier))
                .Select(tp => new RozKpiApiPairInfo()
                {
                    Rooms = tp.Rooms,
                    PairType = tp.Type,
                    IsOnline = tp.IsOnline
                });
            data.PairInfos = teacherPairInfos;
            return data;
        }

        private RozKpiApiGroupPairData FixPairsWithSeveralTeachers(RozKpiApiGroupPairData data)
        {
            var teacherSchedules = GetTeacherSchedules(data);

            var teacherPairs = teacherSchedules.Select(s => s.GetPair(data.Identifier));
            var teacherPairsWithTeachers = teacherPairs.Zip(data.Teachers.SelectMany(t => t));

            var grouppedTeacherPairs = teacherPairsWithTeachers.GroupBy(tp => tp.First.Subject);
            var newTeachers = grouppedTeacherPairs
                .Select(g => g.Select(t => t.Second).ToArray()).ToArray();

            data.Teachers = newTeachers;
            return data;
        }

        private void CopyAllTeachersToFirstPair(ref RozKpiApiGroupPairData data)
        {
            data.Teachers[0] = data.Teachers.SelectMany(t => t).ToArray();
        }

        private void CopySinglePairInfoToAllPairs(ref RozKpiApiGroupPairData data)
        {
            var pairInfoToCopy = data.PairInfos.First();
            var pairsCount = data.SubjectNames.Count();
            data.PairInfos = Enumerable.Repeat(pairInfoToCopy, pairsCount);
        }

        private IEnumerable<RozKpiApiGroupPair> CreatePairsFromData(RozKpiApiGroupPairData data)
        {
            logger.Verbose("Groupping {subjectsCount} subjects, {teachersCount} teachers, {infoCount} pair infos", data.SubjectNames.Count(), data.Teachers.SelectMany(t => t).Count(), data.PairInfos.Count());
            var pairs = new List<RozKpiApiGroupPair>();

            for (int i = 0; i < data.SubjectNames.Count(); i++)
            {
                var subjectName = data.SubjectNames.ElementAt(i);
                var subjectFullName = data.FullSubjectNames.ElementAt(i);
                var teachers = data.Teachers.Any() ? data.Teachers.ElementAt(i) : null;
                var pairInfo = data.PairInfos.Any() ? data.PairInfos?.ElementAt(i) : null;

                var subject = new RozKpiApiSubject()
                {
                    SubjectName = subjectName,
                    SubjectFullName = subjectFullName
                };

                var pair = new RozKpiApiGroupPair()
                {
                    Subject = subject,
                    Teachers = teachers,
                    Rooms = pairInfo?.Rooms ?? Enumerable.Empty<string>().ToList(),
                    IsOnline = pairInfo?.IsOnline ?? false,
                    Type = pairInfo?.PairType ?? Models.PairType.Lecture
                };

                pairs.Add(pair);
            }

            return pairs;
        }

        private bool IsMatchingPairData(RozKpiApiGroupPairData data)
        {
            return data.SubjectNames.Count() == data.Teachers.Count() && data.SubjectNames.Count() == data.PairInfos.Count();
        }

        private bool IsSeveralTeachersPerPair(RozKpiApiGroupPairData data)
        {
            return data.SubjectNames.Count() < data.Teachers.Count() && data.Teachers.Any();
        }

        private bool IsLessPairInfosThanPairs(RozKpiApiGroupPairData data)
        {
            return data.PairInfos.Count() < data.SubjectNames.Count() && data.SubjectNames.Any();
        }

        private bool IsSeveralRoomsPerPair(RozKpiApiGroupPairData data)
        {
            return data.PairInfos.Count() > data.SubjectNames.Count() && data.SubjectNames.Count() == data.Teachers.Count();
        }

        private bool IsLessTeachersThanPairs(RozKpiApiGroupPairData data)
        {
            return data.SubjectNames.Count() > data.Teachers.Count() && data.Teachers.Any();
        }

        private bool IsSinglePairWithSeveralTeachers(RozKpiApiGroupPairData data)
        {
            return IsSeveralTeachersPerPair(data) && data.SubjectNames.Count() == 1;
        }

        private bool AreSomePairsWithSeveralTeachers(RozKpiApiGroupPairData data)
        {
            return IsSeveralTeachersPerPair(data) && data.SubjectNames.Count() > 1;
        }

        private bool AreMultiplePairsWithSinglePairInfo(RozKpiApiGroupPairData data)
        {
            return IsLessPairInfosThanPairs(data) && data.PairInfos.Count() == 1;
        }

        private bool IsNonMatchingPairInfoAndPairsCount(RozKpiApiGroupPairData data)
        {
            return IsLessPairInfosThanPairs(data) && data.PairInfos.Count() > 1;
        }

        private bool IsSinglePairWithSeveralRooms(RozKpiApiGroupPairData data)
        {
            return IsSeveralRoomsPerPair(data) && data.SubjectNames.Count() == 1;
        }

        private bool AreSomePairsWithSeveralRooms(RozKpiApiGroupPairData data)
        {
            return IsSeveralRoomsPerPair(data) && data.SubjectNames.Count() > 1;
        }

        private bool AreSeveralPairsWithOneTeacher(RozKpiApiGroupPairData data)
        {
            return IsLessTeachersThanPairs(data) && data.Teachers.Count() == 1;
        }

        private bool AreMorePairsThanTeachers(RozKpiApiGroupPairData data)
        {
            return IsLessTeachersThanPairs(data) && data.Teachers.Count() > 1;
        }
    }
}
