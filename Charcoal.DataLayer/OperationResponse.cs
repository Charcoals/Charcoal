namespace Charcoal.DataLayer
{
    public class OperationResponse
    {
        public bool HasSucceeded { get; set; }
        public string Description { get; set; }

        public OperationResponse(bool hasSucceeded=false, string description="")
        {
            HasSucceeded = hasSucceeded;
            Description = description;
        }
    }
}