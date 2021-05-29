import requests

from django.conf import settings
from django.shortcuts import render
from django.contrib.auth import get_user_model
from rest_framework import mixins, viewsets, permissions
from rest_framework import status
from .permissions import IsOwnerOrReadOnly
from .serializers import ProfileSerializer


class ProfileViewSet(viewsets.GenericViewSet, mixins.DestroyModelMixin, mixins.RetrieveModelMixin, mixins.UpdateModelMixin):
    queryset = get_user_model().objects.all()
    serializer_class = ProfileSerializer
    permission_classes = [permissions.IsAuthenticated, IsOwnerOrReadOnly]


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
