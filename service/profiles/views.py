import requests

from django.conf import settings
from django.shortcuts import render
from django.contrib.auth import get_user_model
from rest_framework import mixins, viewsets, permissions
from rest_framework import status
from rest_framework.response import Response
from rest_framework.generics import get_object_or_404
from rest_framework.decorators import action

from .permissions import IsOwnerOrReadOnly
from .serializers import ProfileSerializer


class ProfileViewSet(viewsets.GenericViewSet, mixins.DestroyModelMixin, mixins.RetrieveModelMixin, mixins.UpdateModelMixin):
    queryset = get_user_model().objects.all()
    serializer_class = ProfileSerializer
    permission_classes = [permissions.IsAuthenticated, IsOwnerOrReadOnly]

    def get_object(self):
        """
        Returns the object the view is displaying.
        """
        queryset = self.filter_queryset(self.get_queryset())
        obj = get_object_or_404(queryset, email=self.request.user.email)

        # May raise a permission denied
        self.check_object_permissions(self.request, obj)
        return obj

    @action(methods=['GET'], detail=False)
    def info(self, request, *args, **kwargs):
        instance = self.get_object()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(methods=['POST'], detail=False)
    def gain_stellar_points(self, request, *args, **kwargs):
        stellar_points = int(request.data['stellar_points'])

        if stellar_points < 0:
            return Response(status=status.HTTP_400_BAD_REQUEST)

        instance = self.get_object()
        data = {
            'email': request.user.email,
            'stellar_points': instance.stellar_points + stellar_points
        }

        serializer = self.get_serializer(instance, data=data)
        serializer.is_valid(raise_exception=True)
        self.perform_update(serializer)
        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(methods=['POST'], detail=False)
    def high_score(self, request, *args, **kwargs):
        score = int(request.data['score'])
        instance = self.get_object()

        is_high_score = score > instance.high_score

        if is_high_score:
            data = {
                'email': request.user.email,
                'high_score': score
            }
            serializer = self.get_serializer(instance, data=data)
            serializer.is_valid(raise_exception=True)
            self.perform_update(serializer)

        result = {
            'score': score,
            'is_high_score': is_high_score
        }

        return Response(result, status=status.HTTP_200_OK)


def verify(request, *args, **kwargs):
    url = settings.DOMAIN + '/auth/users/activation/'
    response = requests.post(url, kwargs)
    message = 'dafuq'
    print(response.status_code)
    if response.status_code == status.HTTP_204_NO_CONTENT:
        message = 'Your account has been activated succesfully!'
    elif response.status_code == status.HTTP_400_BAD_REQUEST:
        message = 'Invalid activation parameters were provided'
    elif response.status_code == status.HTTP_403_FORBIDDEN:
        message = 'Your acccount has already been activated'

    context = {'message': message}
    return render(request, 'profiles/verification.html', context)
