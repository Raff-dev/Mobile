from django.contrib.auth import get_user_model
from rest_framework_simplejwt.serializers import TokenObtainPairSerializer
from rest_framework import serializers
from .models import Profile


class EmailTokenObtainSerializer(TokenObtainPairSerializer):
    username_field = Profile.EMAIL_FIELD


class ProfileSerializer(serializers.Serializer):
    class Meta:
        model = Profile
        fields = ['email', 'id']


class RegisterSerializer(serializers.ModelSerializer):

    class Meta:
        model = get_user_model()
        fields = ['id', 'email', 'password']
        write_only_fields = ['password']
        read_only_fields = ['id']

    def create(self, validated_data):
        user = get_user_model().objects.create_user(**validated_data)
        return user

    def update(self, instance, validated_data):
        if 'password' in validated_data:
            password = validated_data.pop('password')
            instance.set_password(password)
        return super(RegisterSerializer, self).update(instance, validated_data)
