using UnityEngine;

public static class BonLibrary
{
    public static T[] ShuffleArray<T>(T[] array, System.Random prng)
    {
        int remainingElementCount = array.Length;
        int randomIndex = 0;

        while (remainingElementCount > 1)
        {
            randomIndex = prng.Next(0, remainingElementCount);
            T chosenElement = array[randomIndex];

            remainingElementCount--;

            array[randomIndex] = array[remainingElementCount];
            array[remainingElementCount] = chosenElement;
        }

        return array;
    }

	public static bool CheckIntersection(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2)
	{
		//Taken from https://www.habrador.com/tutorials/math/5-line-line-intersection/

		bool isIntersecting = false;

		float denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

		//Make sure the denominator is > 0, if not the lines are parallel
		if (denominator != 0f)
		{
			float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;
			float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;

			if (u_a > 0f + Mathf.Epsilon && u_a < 1f - Mathf.Epsilon && u_b > 0f + Mathf.Epsilon && u_b < 1f - Mathf.Epsilon)
			{
				isIntersecting = true;
			}
		}

		return isIntersecting;
	}
}
