using UnityEngine;

/**
 * Here are stored information about spectator's movement : position, rotation, speed...
 * It's a SciptableObject so any element can listen to it ; so we can save spectator position throughout several launches ; ...
 * Check https://www.youtube.com/watch?v=raQ3iHhE_Kk to see all benefits
 * Don't forget to create a new ScriptableObject each time you modify this code and to assign the newly created to all the elements that require it
 */
[CreateAssetMenu]
public class SpectatorVariables : ScriptableObject
{
    // We suppose the spectator has the headset on his head at the experiment's launch, so it's well calibrated
    // -- INPUTS --
    public Vector3 initialPosition;
    public Vector3 currentPosition;
    public Vector3 rotation;
    public float speed;

    public float currentHeight;


    // -- THRESHOLDS --
    public Vector2[] distancePercentage = {
        new Vector2(0f, 0f), 
        new Vector2(0f, 0f), 
        new Vector2(0f, 0f), 
        new Vector2(0f, 0f)
    };
    public float interval = 0.10f;

    public GameObject overObject;
    
    // -- OUTPUTS --
    private float currentYRatio;

    /// Enumeration for every posture
    public enum Posture { StandUp, Sit, Crouch, LieDown };

    public Posture posture;

    /// <summary>
    /// Init the height of the body and the array of purcent height
    /// </summary>
    public void SetInitialPosition(Vector3 value)
    {
        initialPosition = value;
        distancePercentage[3] = new Vector2(0f, 0.33f);
        distancePercentage[1] = new Vector2(0.33f, 0.56f);
        distancePercentage[2] = new Vector2(0.56f, 0.90f);
        distancePercentage[0] = new Vector2(0.90f, 1.15f);
    }

    /// <summary>
    /// Update values of the head  
    /// </summary>    
    public void updateBody(Transform transform, float height, GameObject obj){
        currentPosition = transform.position;
        currentHeight = height;
        overObject = obj;
        rotation = transform.rotation.eulerAngles;
        speed = Vector3.Distance(transform.position, currentPosition) * Time.deltaTime;

    }

    /// <summary>
    /// Check the actual posture of the person
    /// </summary>
    public void ComputePosition(float distanceFromObjectBelow)
    {
        for(int i = 0; i < distancePercentage.Length; i++){
            if (distanceFromObjectBelow >= distancePercentage[i][0] * initialPosition.y 
              && distanceFromObjectBelow < distancePercentage[i][1] * initialPosition.y)
            {
                posture = (Posture)i;
            }
        }
        
    }
}