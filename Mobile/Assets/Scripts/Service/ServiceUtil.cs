using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ServiceUtil {
    public const string BASE_URL = "http://localhost:8000";
    public const string PROFILE_URL = BASE_URL + "/Profiles/";
    public const string AUTH_URL = BASE_URL + "/auth";
    public const string REGISTER_URL = AUTH_URL + "/users/";
    public const string LOGIN_URL = AUTH_URL + "/jwt/create/";
    public const string REFRESH_URL = AUTH_URL + "/jwt/refresh/";
    public const string VERIFY_URL = AUTH_URL + "/jwt/verify/";
    public const string USER_URL = AUTH_URL + "/users/me/";

    public const int STATUS_201_CREATED = 201;
    public const int STATUS_200_OK = 200;
    public const int STATUS_400_BAD_REQUEST = 400;
    public const int STATUS_401_UNAUTHORIZED = 401;

    public const string EMAIL_FIELD_NAME = "email";
    public const string PASSWORD_FIELD_NAME = "password";
    public const string PASSWORD2_FIELD_NAME = "re_password";

    public const string REFRESH_FIELD_NAME = "refresh";
    public const string TOKEN_FIELD_NAME = "token";

    public const string ERROR_SOMETING_WRONG = "Something went wrong, check your connection and try later";
    public const string ERROR_UNKNOWN = "Unknown error has occured";

    public const string WARNING_LOGGED_OUT = "You are logged out";

}
