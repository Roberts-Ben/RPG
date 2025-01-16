using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public IEnumerator AttackAnimLerp(int entity, Vector3 targetPosition, float lerpDuration)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        StartCoroutine(AttackAnimReturnLerp(lerpDuration));
    }
    public IEnumerator AttackAnimReturnLerp(float lerpDuration)
    {
        float timeElapsed = 0;
        Vector3 resetPosition = this.GetComponent<BaseClass>().GetStartingPos();

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, resetPosition, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
