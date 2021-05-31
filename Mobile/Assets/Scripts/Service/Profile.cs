using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Profile {

    public static IEnumerator fetchProfileInfo(System.Action<Response> handleResponse) {
        UnityWebRequest request = UnityWebRequest.Get(ServiceUtil.PROFILE_INFO_URL);
        Authorization.setAuthorizationHeader(request);

        yield return request.SendWebRequest();
        Response response = getProfileResponse(request);
        handleResponse(response);
    }

    public static Response getProfileResponse(UnityWebRequest request) {
        return Authorization.getAuthResponse(
            request: request,
            isSuccess: request => request.responseCode == ServiceUtil.STATUS_200_OK,
            getReturn: request => {
                ProfileResponse response = JsonUtility.FromJson<ProfileResponse>(request.downloadHandler.text);
                return DataResponse<ProfileResponse>.ok(data: response);
            }
        );
    }

    [Serializable]
    public class ProfileResponse {
        public int stellar_points;
        public int high_score;
        public bool is_high_score;
        public int score;

        public string toString() {
            return $"Stellar Points: {stellar_points} High Score: {is_high_score} Score: {score}";
        }
    }
}
