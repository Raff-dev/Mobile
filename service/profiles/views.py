from django.contrib.auth import get_user_model
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from rest_framework import mixins, viewsets, permissions, status

from .permissions import IsOwnerOrReadOnly
from .serializers import EmailTokenObtainSerializer, ProfileSerializer, RegisterSerializer
from .models import Profile


class EmailTokenObtainPairView(TokenObtainPairView):
    serializer_class = EmailTokenObtainSerializer


class RegisterViewSet(viewsets.GenericViewSet, mixins.CreateModelMixin):
    serializer_class = RegisterSerializer
    permission_classes = [permissions.AllowAny]

    # def create(self, request, *args,  **kwargs):
    #     serializer = self.get_serializer(data=request.data)
    #     if serializer.is_valid():
    #         user = serializer.save()

    #         result = {
    #             "profile": self.get_serializer(user, context=self.get_serializer_context()).data,
    #             "message": "User Created Successfully.  Now perform Login to get your token",
    #         }
    #         return Response(data=result, status=status.HTTP_201_CREATED)
    #     return Response(status=status.HTTP_409_CONFLICT)


class ProfileViewSet(viewsets.GenericViewSet, mixins.DestroyModelMixin, mixins.RetrieveModelMixin):
    queryset = get_user_model().objects.all()
    serializer_class = ProfileSerializer
    permission_classes = [permissions.IsAuthenticated, IsOwnerOrReadOnly]
