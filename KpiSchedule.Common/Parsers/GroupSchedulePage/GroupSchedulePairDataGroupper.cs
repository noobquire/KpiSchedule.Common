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

        public GroupSchedulePairDataGroupper(ILogger logger)
        {
            this.logger = logger;
        }

        public IEnumerable<RozKpiApiGroupPair> GroupPairData(RozKpiApiGroupPairData data)
        {
            if (IsMatchingPairData(data))
            {
                // no fixing needed, create and return
                return CreatePairsFromData(data);
            }

            if (IsSinglePairWithSeveralTeachers(data))
            {
                CopyAllTeachersToFirstPair(ref data);
            }

            if(AreSomePairsWithSeveralTeachers(data))
            {
                // TODO: rename handlers like this to reflect what they are doing
                HandlePairsWithSeveralTeachers(ref data);
            }

            if(AreMultiplePairsWithSinglePairInfo(data))
            {
                CopySinglePairInfoToAllPairs(ref data);
            }

            if(IsNonMatchingPairInfoAndPairsCount(data))
            {
                HandleNonMatchingPairInfoAndPairsCount(ref data);
            }

            if(IsSinglePairWithSeveralRooms(data))
            {
                CopyAllRoomsToFirstPair(ref data);
            }

            if(AreSomePairsWithSeveralRooms(data))
            {
                HandleSomePairsWithSeveralRooms(ref data);
            }

            if(AreSeveralPairsWithOneTeacher(data))
            {
                CopyFirstTeacherToAllPairs(ref data);
            }

            if(AreMorePairsThanTeachers(data))
            {
                HandleMorePairsThanTeachers(ref data);
            }

            return CreatePairsFromData(data);
        }

        private void HandleMorePairsThanTeachers(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
        }

        private void CopyFirstTeacherToAllPairs(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
        }

        private void HandleSomePairsWithSeveralRooms(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
        }

        private void CopyAllRoomsToFirstPair(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
        }

        private void HandleNonMatchingPairInfoAndPairsCount(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
        }

        private void HandlePairsWithSeveralTeachers(ref RozKpiApiGroupPairData data)
        {
            throw new NotImplementedException();
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
            var pairs = new List<RozKpiApiGroupPair>();

            for(int i = 0; i < data.SubjectNames.Count(); i++)
            {
                var subjectName = data.SubjectNames.ElementAt(i);
                var subjectFullName = data.FullSubjectNames.ElementAt(i);
                var teachers = data.Teachers.ElementAt(i);
                var pairInfo = data.PairInfos.ElementAt(i);

                var subject = new RozKpiApiSubject()
                {
                    SubjectName = subjectName,
                    SubjectFullName = subjectFullName
                };

                var pair = new RozKpiApiGroupPair()
                {
                    Subject = subject,
                    Teachers = teachers,
                    Rooms = pairInfo.Rooms,
                    IsOnline = pairInfo.IsOnline,
                    Type = PairTypeParser.ParsePairType(pairInfo.PairType)
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
