public class Response {
    public const bool SUCCESS = false;
    public const bool ERROR = true;
    public bool isError;

    protected Response(bool isError) {
        this.isError = isError;
    }

    public static Response ok() {
        return new Response(SUCCESS);
    }

    public static Response error() {
        return new Response(ERROR);
    }
}
