using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct Booking
    {
        public Transform spawnPoint;
        public Recipe order;
        public float enterTime;
        public float patience;
        public GameObject alternativeCustomerOrientation;

        public Booking(Transform _transform, Recipe _recipe = null, float _patience = 35f, float _enterTime = 0f)
        {
            spawnPoint = _transform;
            patience = _patience;
            order = _recipe;
            enterTime = _enterTime;
            alternativeCustomerOrientation = null;
        }
    }

    public class CustomerBookings : MonoBehaviour
    {
        private float elapsedTime = 0;
        private int index = 0;
        public GameObject customerPrefab;
        public Recipe defaultRecipe;
        public float timeTillLoop = 5f;
        public List<Booking> bookings = new List<Booking>();


        private void Update()
        {
            elapsedTime += Time.deltaTime;

            if (index < bookings.Count)
            {
                Booking curBooking = bookings[index];

                if (curBooking.enterTime < elapsedTime)
                {
                    SpawnCustomerFromBooking(curBooking);
                    index++;
                    if (index >= bookings.Count) elapsedTime = 0;
                }
            }
            else
            {
                if (elapsedTime > timeTillLoop)
                {
                    index = 0;
                    elapsedTime = 0;
                }
            }
        }

        [ContextMenu("Add new booking")]
        public void AddNewBooking()
        {
            bookings.Add(new Booking(transform, defaultRecipe));
        }

        [ContextMenu("duplicate booking")]
        public void DuplicateNewBooking()
        {
            bookings.Add(bookings[bookings.Count - 1]);
        }

        void SpawnCustomerFromBooking(Booking booking)
        {
            GameObject prefabToUse = booking.alternativeCustomerOrientation;
            if (!prefabToUse) prefabToUse = customerPrefab;
            GameObject newCustomer =
                Instantiate(prefabToUse, booking.spawnPoint.position, Quaternion.identity, transform);
            CustomerController cust = newCustomer.GetComponent<CustomerController>();
            if (cust)
            {
                cust.order = booking.order;
                cust.patience = booking.patience;
            }
        }
    }
}