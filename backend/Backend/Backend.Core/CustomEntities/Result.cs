namespace Backend.Core.CustomEntities
{
    public class Result
    {
        public bool Success { get; set; }
        public List<Message> Messages { get; set; }

        protected Result() { }

        protected Result(bool success, string? messageCode)
        {
            Success = success;

            if (string.IsNullOrEmpty(messageCode))
            {
                Messages = new List<Message>();
                return;
            }

            Messages = new List<Message> { new Message(messageCode) };
        }

        protected Result(bool success, List<string> messageCodes)
        {
            Success = success;

            if (messageCodes.Count == 0)
            {
                Messages = new List<Message>();
                return;
            }

            Messages = new List<Message>(messageCodes.Select(c => new Message(c)));
        }

        public static Result Ok() => new Result(true, "");
        public static Result Ok(List<string> messageCodes) => new Result(true, messageCodes);

        public static Result Fail(string? error) => new Result(false, error);
        public static Result Fail(List<string> errors) => new Result(false, errors);
        public static Result FailFrom(Result originResult) => new Result(false, originResult.Messages.Select(c => c.Code).ToList()!);
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        public Result() { }

        protected Result(bool success, T value, string? messageCode) : base(success, messageCode)
        {
            Value = value;
        }

        protected Result(bool success, T value, List<string> messageCodes) : base(success, messageCodes)
        {
            Value = value;
        }

        public static Result<T> Ok(T value) => new Result<T>(true, value, "");
        public static Result<T> Ok(T value, List<string> messageCodes) => new Result<T>(true, value, messageCodes);

        public new static Result<T> Fail(string? error) => new Result<T>(false, default, error);
        public new static Result<T> Fail(List<string> errors) => new Result<T>(false, default, errors);
        public new static Result<T> FailFrom(Result originResult) => new Result<T>(false, default, originResult.Messages.Select(c => c.Code).ToList()!);
    }

    public class Message
    {
        public string? Code { get; }
        public string Text { get; private set; }

        public Message(string? code)
        {
            Code = code;
            Text = "";
        }

        public void SetText(string text)
        {
            Text = text;
        }
    }
}
