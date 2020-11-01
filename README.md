#Базовое приложение asp.net Core

Порядок действий
1) Включить docker и kubernetes в docker desktop
2) Установить пакеты kubernetes
    1) Установить [helm](https://helm.sh/docs/intro/install/)
    2) Добавить репозиторий контейнеров
    ```helm repo add stable https://kubernetes-charts.storage.googleapis.com/```
    ```helm repo add jetstack https://charts.jetstack.io```
    ```helm repo update```
    3) Создаем namespace cert-manager
    ```kubectl create ns cert-manager``
    4) Установить ingress и cert-manager
    ```helm install ingress stable/nginx-ingress```
    ```helm install cert-manager jetstack/cert-manager --namespace cert-manager````
3) Запустить регистр контейнеров [(инструкция)](DockerRegistry)
4) Запустить админку kubernetes
    1) Установить ```kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.4/aio/deploy/recommended.yaml```
    2) Запустить ```kubectl proxy```
    3) Узнать токен ```kubectl describe secret```
    4) Зайти http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/
5) Собоать проект в локальный репозиторий контейнеров.
6) Настроить ingress
    1) Добавляем namespace
    ```kubectl apply -f namespace.yaml```
    2) Запускаем deployment
    ```kubectl apply -f Backend/deployment.yaml```
    3) Запускаем сервис
    ```kubectl apply -f Backend/service.yaml```
    3) Запускаем ingress
    ```kubectl apply -f ingress.yaml```