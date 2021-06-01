from typing import List
from profiles.models import Profile, Skin, SkinOwnership
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
from .serializers import ProfileSerializer, SkinSerializer


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
        profile = self.get_object()
        serializer = self.get_serializer(profile)
        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(methods=['POST'], detail=False)
    def gain_stellar_points(self, request, *args, **kwargs):
        stellar_points = int(request.data['stellar_points'])

        if stellar_points < 0:
            return Response(status=status.HTTP_400_BAD_REQUEST)

        profile = self.get_object()
        data = {
            'email': request.user.email,
            'stellar_points': profile.stellar_points + stellar_points
        }

        serializer = self.get_serializer(profile, data=data)
        serializer.is_valid(raise_exception=True)
        self.perform_update(serializer)
        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(methods=['POST'], detail=False)
    def high_score(self, request, *args, **kwargs):
        score = int(request.data['score'])
        profile = self.get_object()

        is_high_score = score > profile.high_score

        if is_high_score:
            data = {
                'email': request.user.email,
                'high_score': score
            }
            serializer = self.get_serializer(profile, data=data)
            serializer.is_valid(raise_exception=True)
            self.perform_update(serializer)

        result = {
            'score': score,
            'is_high_score': is_high_score
        }

        return Response(result, status=status.HTTP_200_OK)

    @action(methods=['POST'], detail=False)
    def unlock_skin(self, request, *args, **kwargs):
        name: str = request.data['name']

        skins: List[Skin] = Skin.objects.all()
        skin: Skin = get_object_or_404(skins, name=name)
        profile: Profile = self.get_object()

        if profile.skins.filter(skin=skin).exists() or profile.stellar_points < skin.price:
            return Response(status=status.HTTP_400_BAD_REQUEST)

        profile.stellar_points -= skin.price
        SkinOwnership.objects.create(profile=profile, skin=skin)
        profile.save()
        result = {'stellar_points': profile.stellar_points}
        return Response(result, status=status.HTTP_201_CREATED)


class SkinViewSet(viewsets.ModelViewSet):
    queryset = Skin.objects.all()
    serializer_class = SkinSerializer


def verify(request, *args, **kwargs):
    url = settings.DOMAIN + '/auth/users/activation/'
    response = requests.post(url, kwargs)
    message = ''
    print(response.status_code)
    if response.status_code == status.HTTP_204_NO_CONTENT:
        message = 'Your account has been activated succesfully!'
    elif response.status_code == status.HTTP_400_BAD_REQUEST:
        message = 'Invalid activation parameters were provided'
    elif response.status_code == status.HTTP_403_FORBIDDEN:
        message = 'Your acccount has already been activated'

    context = {'message': message}
    return render(request, 'profiles/verification.html', context)
