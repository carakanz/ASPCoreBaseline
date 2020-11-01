#Базовое приложение asp.net Core

Порядок действий
1) Включить docker и kubernetes в docker desktop
2) Запустить регистр контейнеров [this subtext](DockerRegistry/README.md)
3) Запустить админку kubernetes
    1) Установить '''kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.4/aio/deploy/recommended.yaml'''
    2) Запустить '''kubectl proxy'''
    3) Узнать токен '''kubectl describe secret'''
    4) Зайти http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/overview?namespace=default