from rest_framework import routers

from .views import ProfileViewSet

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')

urlpatterns = router.urls + [

]
