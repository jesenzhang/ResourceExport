using UnityEngine;
using System.Collections.Generic;

public static class FracturedChunkExt
{
    public static void Impact(this FracturedChunk chunk,Vector3 force,Vector3 pos,float radius)
    {
        if (chunk.GetComponent<Rigidbody>() != null && chunk.IsSupportChunk == false)
        {
            List<FracturedChunk> listBreaks = new List<FracturedChunk>();

            if (chunk.IsDetachedChunk == false)
            {
                // Compute random list of connected chunks that are detaching as well (we'll use the ConnectionStrength parameter).
                listBreaks = chunk.ComputeRandomConnectionBreaks();
                listBreaks.Add(chunk);
                chunk.DetachFromObject();

                foreach (FracturedChunk breakChunk in listBreaks)
                {
                    breakChunk.DetachFromObject();
                    breakChunk.GetComponent<Rigidbody>().AddForceAtPosition(force, pos, ForceMode.Impulse);
                }
            }

            List<FracturedChunk> listRadius = chunk.FracturedObjectSource.GetDestructibleChunksInRadius(pos, radius, true);

            foreach (FracturedChunk breakChunk in listRadius)
            {
                breakChunk.DetachFromObject();
                breakChunk.GetComponent<Rigidbody>().AddForceAtPosition(force, pos, ForceMode.Impulse);
            }
        }

        // Even if it is support chunk, play the sound and instance the prefabs

        chunk.FracturedObjectSource.NotifyImpact(pos);
    }
}
