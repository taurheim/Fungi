using UnityEngine;

public class NodeOutline : MonoBehaviour {
  private bool isPulsing = false;

  private Color primaryOutlineColor = Color.yellow;
  private Color pulseOutlineColor = Color.red;

  private Vector3 pulseScale;

  private Vector3 normalScale;

  public float pulseSpeed = 1.5f;
  public float pulseAmount = 1.4f;
  private float pulseStartTime;
  private float journeyLength;

  void Start() {
    normalScale = transform.localScale; // To
    setColor(primaryOutlineColor);
  }

  void Update() {
    if (isPulsing) {
      float scaleCovered = (Time.time - pulseStartTime) * pulseSpeed;
      float fracJourney = scaleCovered / journeyLength;
      transform.localScale = Vector3.Lerp(pulseScale, normalScale, fracJourney);

      if (transform.localScale == normalScale) {
        isPulsing = false;
        setColor(primaryOutlineColor);
      }
    }
  }

  public void pulse() {
    Debug.Log("Starting to pulse");
    isPulsing = true;
    
    // Figure out what we're pulsing out to (don't pulse in the y direction)
    pulseScale = Vector3.Scale(normalScale, new Vector3(pulseAmount, 0f, pulseAmount));
    transform.localScale = pulseScale; // From

    // Lerp setup
    pulseStartTime = Time.time;
    journeyLength = Vector3.Distance(pulseScale, normalScale);
    setColor(pulseOutlineColor);
  }

  private void setColor(Color color) {
    GetComponent<Renderer>().material.color = color;
  }
}