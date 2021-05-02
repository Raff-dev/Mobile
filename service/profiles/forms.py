from django.forms import ModelForm

from .models import Profile


class ProfileForm(ModelForm):
    class Meta:
        models = Profile
        exclude = ['username']
