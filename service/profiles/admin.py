from django.contrib import admin
from django.contrib.auth.admin import UserAdmin

from .models import Profile
from .forms import ProfileForm


class ProfileAdmin(UserAdmin):
    ordering = ['email']
    form = ProfileForm


admin.site.register(Profile, ProfileAdmin)
