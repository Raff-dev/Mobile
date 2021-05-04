from django.urls import path, include
from rest_framework import routers

from django.urls import path, include

from .views import ProfileViewSet

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')

urlpatterns = router.urls + [

]
