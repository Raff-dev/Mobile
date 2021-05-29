using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMessage {
    public string message;
    public bool isError;
    public string data;

    public Color ERROR_COLOR = UnityEngine.Color.red;
    public Color MESSAGE_COLOR = UnityEngine.Color.blue;

    public const bool SUCCESS = false;
    public const bool ERROR = true;

    public ResponseMessage(string message, bool isError, string data) {
        this.message = message;
        this.isError = isError;
        this.data = data;
    }

    public ResponseMessage(string message, bool isError) {
        this.message = message;
        this.isError = isError;
    }

    public ResponseMessage() {
        this.isError = false;
    }

    public void show(TMPro.TMP_Text messageText) {
        Color color = isError ? ERROR_COLOR : MESSAGE_COLOR;
        messageText.color = ERROR_COLOR;
        messageText.text = message;
    }
}
