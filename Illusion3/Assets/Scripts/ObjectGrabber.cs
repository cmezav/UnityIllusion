using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Configuracion")]
    public float distanciaReferencia = 4f;
    public float distanciaMaxima = 10f;
    public float distanciaMinima = 1f;
    public float velocidadRueda = 0.5f;
    public LayerMask layerGrabbable;

    private Camera camara;
    private GameObject objetoAgarrado;
    private Vector3 escalaOriginal;
    private float distanciaInicial;
    private float distanciaActual;

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
        {
            AjustarDistancia();
            MantenerObjeto();
        }
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
            distanciaActual = hit.distance;

            rbAgarrado = objetoAgarrado.GetComponent<Rigidbody>();
            colliderAgarrado = objetoAgarrado.GetComponent<Collider>();

            if (rbAgarrado != null) rbAgarrado.isKinematic = true;
            if (colliderAgarrado != null) colliderAgarrado.enabled = false;

            objetoAgarrado.layer = LayerMask.NameToLayer("Held");
        }
    }

    void AjustarDistancia()
    {
        float rueda = Input.GetAxis("Mouse ScrollWheel");
        distanciaActual += rueda * velocidadRueda * 10f;
        distanciaActual = Mathf.Clamp(distanciaActual, distanciaMinima, distanciaMaxima);
    }

    void MantenerObjeto()
    {
        Ray ray = camara.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 posicionObjetivo = ray.origin + ray.direction * distanciaActual;

        objetoAgarrado.transform.position = posicionObjetivo;

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