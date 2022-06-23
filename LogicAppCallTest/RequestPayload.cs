using System.Collections.Generic;

namespace LogicAppCallTest
{
    internal class RequestPayload
    {
        public RequestPayload()
        {
            relations = new List<EntityRelations>();
        }
        public string assignedToEntityId { get; set; }
        public string assignmentType { get; set; }
        public string comment { get; set; }
        public string correlationId { get; set; }
        public string createdBy { get; set; }
        public string data { get; set; }
        public string externalId { get; set; }
        public string fourEyeSubjectId { get; set; }
        public string operationId { get; set; }
        public string sourceId { get; set; }
        public string sourceName { get; set; }
        public string status { get; set; }
        public string subject { get; set; }
        public int taskType { get; set; }
        public IEnumerable<EntityRelations> relations { get; set; }
    }

    public class EntityRelations
    {
        public string entityId { get; set; }
        public string entityType { get; set; }
    }
}
