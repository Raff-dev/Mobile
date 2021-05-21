from django.contrib.auth import get_user_model
from rest_framework import serializers
from .models import Profile, Skin


class ProfileSerializer(serializers.ModelSerializer):
    class Meta:
        model = Profile
        exclude = ['password']


class SkinSerializer(serializers.ModelSerializer):
    class Meta:
        model = Skin
        fields = '__all__'


class RegisterSerializer(serializers.ModelSerializer):

    class Meta:
        model = get_user_model()
        fields = ['id', 'email', 'password', 'is_active']
        write_only_fields = ['password']
        read_only_fields = ['id']
