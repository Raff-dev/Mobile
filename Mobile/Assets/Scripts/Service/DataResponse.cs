public class DataResponse<T> : Response {
    public T data;

    public DataResponse(bool isError, T data) : base(isError) {
        this.data = data;
    }

    public static DataResponse<T> ok(T data) {
        return new DataResponse<T>(SUCCESS, data);
    }

    public static DataResponse<T> from(Response response) {
        return (DataResponse<T>)response;
    }
}
