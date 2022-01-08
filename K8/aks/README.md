# LogCorner.EduSync.Notification.Server

az acr login --name acrlogcorner

kubectl create secret docker-registry regsecret --docker-server=acrlogcorner.azurecr.io --docker-username=acrlogcorner  --docker-password=6eQtd1P+x70yfXMENj8GhpWB4GIcjRWf  --docker-email=admin@azurecr.io  

docker tag  logcornerhub/logcorner-edusync-signalr-server  acrlogcorner.azurecr.io/logcorner-edusync-signalr-server 
docker push acrlogcorner.azurecr.io/logcorner-edusync-signalr-server 


kubectl get secret regsecret --output=yaml
kubectl get secret regcred --output="jsonpath={.data.\.dockerconfigjson}" | base64 --decode




command ==> 

docker tag  logcornerhub/logcorner-edusync-speech-command   acrlogcorner.azurecr.io/logcorner-edusync-speech-command 
docker push acrlogcorner.azurecr.io/logcorner-edusync-speech-command 



docker tag  logcornerhub/logcorner-edusync-speech-command-data   acrlogcorner.azurecr.io/logcorner-edusync-speech-command-data 
docker push acrlogcorner.azurecr.io/logcorner-edusync-speech-command-data 

query ==>
docker tag  logcornerhub/logcorner-edusync-speech-query    acrlogcorner.azurecr.io/logcorner-edusync-speech-query
docker push acrlogcorner.azurecr.io/logcorner-edusync-speech-query



front ==>
docker tag  logcornerhub/logcorner-edusync-speech-query    acrlogcorner.azurecr.io/logcorner-edusync-speech-query
docker push acrlogcorner.azurecr.io/logcorner-edusync-speech-query