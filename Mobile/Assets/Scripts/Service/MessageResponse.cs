using UnityEngine;

public class MessageResponse : Response {
    public const string ERROR_LOGGED_OUT = "You are logged out";
    public const string ERROR_CONNECTION = "Something went wrong, check your connection and try later";
    public const string ERROR_UNKNOWN = "Unknown error has occured";
    public const string ERROR_BAD_REQUEST = "Bad request";
    public const string ERROR_NOT_FOUND = "Not Found";

    public Color ERROR_COLOR = UnityEngine.Color.red;
    public Color MESSAGE_COLOR = UnityEngine.Color.blue;

    public string message;

    protected MessageResponse(string message, bool isError) : base(isError) {
        this.message = message;
    }

    public void show(TMPro.TMP_Text messageText) {
        Color color = isError ? ERROR_COLOR : MESSAGE_COLOR;
        messageText.color = ERROR_COLOR;
        messageText.text = message;
    }

    public static MessageResponse from(Response response) {
        return (MessageResponse)response;
    }

    public static MessageResponse loggedOutError() {
        return new MessageResponse(ERROR_LOGGED_OUT, ERROR);
    }

    public static MessageResponse unauthorizedError(string message) {
        return new MessageResponse(message, ERROR);
    }

    public static MessageResponse ok(string message = null) {
        return new MessageResponse(null, SUCCESS);
    }

    public static MessageResponse validationError(string message = null) {
        return new MessageResponse(message, ERROR);
    }
    public static MessageResponse connectionError() {
        return new MessageResponse(ERROR_CONNECTION, ERROR);
    }

    public static MessageResponse badRequestError() {
        return new MessageResponse(ERROR_BAD_REQUEST, ERROR);
    }

    public static MessageResponse notFoundError() {
        return new MessageResponse(ERROR_NOT_FOUND, ERROR);
    }

    public static MessageResponse unknownError() {
        return new MessageResponse(ERROR_UNKNOWN, ERROR);
    }
}
