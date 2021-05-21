from rest_framework import routers

from .views import ProfileViewSet, SkinViewSet

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')
router.register('SkinViewSet', SkinViewSet, basename='Skins')

urlpatterns = router.urls + [

]
