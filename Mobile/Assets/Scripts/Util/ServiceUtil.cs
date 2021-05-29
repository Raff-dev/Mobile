using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceUtil : MonoBehaviour {
    public const string BASE_URL = "http://localhost:8000";
    public const string AUTH_URL = BASE_URL + "/auth";
    public const string REGISTER_URL = AUTH_URL + "/users/";
    public const string USER_URL = AUTH_URL + "/users/me/";
    public const string PROFILE_URL = BASE_URL + "/Profiles/";

    public const int STATUS_201_CREATED = 201;
    public const int STATUS_400_BAD_REQUEST = 400;
}
