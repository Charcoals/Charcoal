namespace Charcoal.DataLayer
{
    public class DatabaseOperationResponse
    {
        public bool HasSucceeded { get; private set; }
        public string Description { get; private set; }
        public FailReason Reason { get; private set; }
        public dynamic Object { get; set; }

        public DatabaseOperationResponse(bool hasSucceeded=false, string description="", FailReason reason= FailReason.Undefined)
        {
            HasSucceeded = hasSucceeded;
            Description = description;
            Reason = reason;
        }
    }

    public enum FailReason{Undefined=0, ItemAlreadyExist=1, ItemNoLongerExists=2, Exception=3}
}