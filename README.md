#������� ���������� asp.net Core

������� ��������
1) �������� docker � kubernetes � docker desktop
2) ��������� ������� ����������� [this subtext](DockerRegistry/README.md)
3) ��������� ������� kubernetes
    1) ���������� '''kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.4/aio/deploy/recommended.yaml'''
    2) ��������� '''kubectl proxy'''
    3) ������ ����� '''kubectl describe secret'''
    4) ����� http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/overview?namespace=default