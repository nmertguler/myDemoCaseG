using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class AttackPattern : MonoBehaviour
{
    public List<Vector3> GetPattern(Vector3 targetPosition, Vector3 moveDirection, int soldierCount, float spacing)
    {
        // pattern type control
        const int singleRowLimit = 4;
        const float depthFactor = .8F;

        bool isSingleRow = soldierCount <= singleRowLimit;

        moveDirection = moveDirection.normalized;

        // Vector perpendicular to the direction of motion in the X-Z plane
        Vector3 perpendicularDirection = new Vector3(-moveDirection.z, 0, moveDirection.x);

        int halfCount = soldierCount / 2;

        List<Vector3> movementTargets = new List<Vector3>(soldierCount);
        for (int i = 0; i < soldierCount; i++)
        {
            int index = i - halfCount;
            Vector3 spawnPosition = targetPosition + (perpendicularDirection * index * spacing);

            // Adding depth to the inverted V shape
            if (!isSingleRow)
            {
                spawnPosition += moveDirection * Mathf.Abs(index) * spacing * depthFactor;
            }

            movementTargets.Add(spawnPosition);
        }

        return movementTargets;
    }

    
}
