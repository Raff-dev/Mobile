from django.contrib.auth import get_user_model
from rest_framework import mixins, viewsets, permissions

from .permissions import IsOwnerOrReadOnly
from .models import Skin
from .serializers import ProfileSerializer, SkinSerializer


class ProfileViewSet(viewsets.GenericViewSet, mixins.DestroyModelMixin, mixins.RetrieveModelMixin):
    queryset = get_user_model().objects.all()
    serializer_class = ProfileSerializer
    permission_classes = [permissions.IsAuthenticated, IsOwnerOrReadOnly]


class SkinViewSet(viewsets.GenericViewSet, mixins.CreateModelMixin, mixins.RetrieveModelMixin):
    queryset = Skin.objects.all()
    serializer_class = SkinSerializer
    permission_classes = [permissions.IsAuthenticated, IsOwnerOrReadOnly]
