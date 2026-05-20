using UnityEngine;
using UnityEngine.InputSystem;
public class FistWeapon : MonoBehaviour
{
    InputAction smash;
    [SerializeField] private Vector2 smashColliderSize = new Vector2(1.8f, 1f);
    [SerializeField] private Vector2 smashColliderOffset = new Vector2(0,0.8f);
    [SerializeField] private float smashForce = 10f;
    private RaycastHit2D[] smashHits;
    bool smashActive;
    private float animationTime;
    [SerializeField] private float animationSpeed = 2f;
    [SerializeField] private float armExtension = 0.5f;
    [SerializeField] private GameObject fistSprite;
    private Vector2 originalFistPosition;
    [SerializeField] private LineRenderer fistRope;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smash = InputSystem.actions.FindAction("Smash");
        if (smash == null)
        {
            Debug.LogError("Smash action not found in Input System.");
        }
        originalFistPosition = fistSprite.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (smash.WasPressedThisFrame() && animationTime <= 0)
        {
            smashActive = true;

        }
        animationTime -= Time.deltaTime * animationSpeed;
        animationTime = Mathf.Clamp01(animationTime);
        fistSprite.transform.localPosition = Vector2.Lerp(originalFistPosition, originalFistPosition + (Vector2.up * armExtension), animationTime);
        fistRope.SetPosition(1, new Vector3(0, armExtension* animationTime, 0));

    }
    private void FixedUpdate()
    {
        if(smashActive)
        {
            smashHits = Physics2D.CapsuleCastAll((transform.rotation * smashColliderOffset) + transform.position, smashColliderSize, CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z, transform.rotation * smashColliderOffset, 1f);
            Debug.DrawLine((transform.rotation * smashColliderOffset) + transform.position, (transform.rotation * (smashColliderOffset + Vector2.up)) + transform.position, Color.red, 1f);
            if (smashHits != null)
            {
                foreach (var hit in smashHits)
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    if(hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce((hit.transform.position - ((transform.rotation * smashColliderOffset) + transform.position)).normalized * smashForce, ForceMode2D.Impulse);
                        hit.rigidbody.AddTorque((hit.transform.position - ((transform.rotation * smashColliderOffset) + transform.position)).normalized.x * smashForce * Random.Range(-1f, 1f), ForceMode2D.Impulse);
                    }
                }
                smashHits = null;
            }
            animationTime = 1f;


            smashActive = false;
        }
        
    }
}
