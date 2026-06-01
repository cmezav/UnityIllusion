using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Configuracion")]
    public float distanciaReferencia = 4f;
    public float distanciaMaxima = 10f;
    public LayerMask layerGrabbable;

    private Camera camara;
    private GameObject objetoAgarrado;
    private Vector3 escalaOriginal;
    private float distanciaInicial;

    private Rigidbody rbAgarrado;
    private Collider colliderAgarrado;

    void Start()
    {
        foreach (Transform hijo in transform)
        {
            if (hijo.GetComponent<Camera>() != null)
            {
                camara = hijo.GetComponent<Camera>();
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objetoAgarrado == null)
                IntentarAgarrar();
            else
                Soltar();
        }

        if (objetoAgarrado != null)
            MantenerObjeto();
    }

    void IntentarAgarrar()
    {
        Ray ray = camara.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaMaxima, layerGrabbable))
        {
            objetoAgarrado = hit.collider.gameObject;
            escalaOriginal = objetoAgarrado.transform.localScale;
            distanciaInicial = hit.distance;

            rbAgarrado = objetoAgarrado.GetComponent<Rigidbody>();
            colliderAgarrado = objetoAgarrado.GetComponent<Collider>();

            if (rbAgarrado != null) rbAgarrado.isKinematic = true;
            if (colliderAgarrado != null) colliderAgarrado.enabled = false;

            objetoAgarrado.layer = LayerMask.NameToLayer("Held");
        }
    }

    void MantenerObjeto()
    {
        Ray ray = camara.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Vector3 posicionObjetivo;

        if (Physics.Raycast(ray, out hit, distanciaMaxima))
            posicionObjetivo = hit.point;
        else
            posicionObjetivo = ray.origin + ray.direction * distanciaReferencia;

        objetoAgarrado.transform.position = posicionObjetivo;

        float distanciaActual = Vector3.Distance(camara.transform.position, posicionObjetivo);
        float factor = distanciaActual / distanciaInicial;
        objetoAgarrado.transform.localScale = escalaOriginal * factor;
    }

    void Soltar()
    {
        objetoAgarrado.layer = LayerMask.NameToLayer("Grabbable");

        if (rbAgarrado != null) rbAgarrado.isKinematic = false;
        if (colliderAgarrado != null) colliderAgarrado.enabled = true;

        objetoAgarrado = null;
        rbAgarrado = null;
        colliderAgarrado = null;
    }
}