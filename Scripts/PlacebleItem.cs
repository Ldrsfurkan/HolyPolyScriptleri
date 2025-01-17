using System.Collections.Generic;
using UnityEngine;

public class PlacebleItem : MonoBehaviour
{
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOverlappingItems;
    public bool isValidToBeBuilt;
 
    [SerializeField] BoxCollider solidCollider;
 
 
    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }
 
 
        var boxHeight = transform.lossyScale.y;
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight * 0.5f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
 
    }
 
    #region || --- On Triggers --- |
    private void OnTriggerEnter(Collider other)
    {
        if (PlacementSystem.Instance.inPlacementMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;
 
                isGrounded = true;
            }
        }
    }
    #endregion
 
    private void OnTriggerExit(Collider other)
    {
        if (PlacementSystem.Instance.inPlacementMode)//other.CompareTag("Ground") && 
        {
            isGrounded = false;
        }
 
        if (PlacementSystem.Instance.inPlacementMode) // other.CompareTag("Tree") || other.CompareTag("pickable") && 
        {
            isOverlappingItems = false;
        }
    }
 
}