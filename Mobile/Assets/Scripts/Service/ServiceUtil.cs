using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ServiceUtil {
    public const string BASE_URL = "http://localhost:8000/";
    public const string AUTH_URL = BASE_URL + "auth/";

    public const string REGISTER_URL = AUTH_URL + "users/";
    public const string USER_URL = AUTH_URL + "users/me/";

    public const string LOGIN_URL = AUTH_URL + "jwt/create/";
    public const string REFRESH_URL = AUTH_URL + "jwt/refresh/";
    public const string VERIFY_URL = AUTH_URL + "jwt/verify/";

    public const string PROFILE_URL = BASE_URL + "Profiles/";
    public const string PROFILE_INFO_URL = PROFILE_URL + "info/";
    public const string STELLAR_POINTS_GAIN_URL = PROFILE_URL + "gain_stellar_points/";
    public const string HIGH_SCORE_URL = PROFILE_URL + "high_score/";
    public const string UNLOCK_SKIN_URL = PROFILE_URL + "unlock_skin/";

    public const string SKINS_LIST_URL = BASE_URL + "Skins/";

    public const int STATUS_200_OK = 200;
    public const int STATUS_201_CREATED = 201;
    public const int STATUS_400_BAD_REQUEST = 400;
    public const int STATUS_401_UNAUTHORIZED = 401;
    public const int STATUS_404_NOT_FOUND = 404;

    public const string EMAIL_FIELD_NAME = "email";
    public const string PASSWORD_FIELD_NAME = "password";
    public const string PASSWORD2_FIELD_NAME = "re_password";

    public const string REFRESH_FIELD_NAME = "refresh";
    public const string TOKEN_FIELD_NAME = "token";
}
