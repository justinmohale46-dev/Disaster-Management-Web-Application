using System.Text;
using GiftOfTheGivers.Models;

namespace GiftOfTheGivers.Services
{
    public class ReceiptPdfService
    {
        public byte[] GenerateDonationReceipt(Donation donation)
        {
            // Create a simple text receipt
            var receiptContent = $@"
GIFT OF THE GIVERS FOUNDATION
==============================

DONATION RECEIPT
================

Receipt Number: D-{donation.DonationID:D5}
Date: {donation.DonationDate:MMMM dd, yyyy}
Donor: {donation.User.FirstName} {donation.User.LastName}

DONATION DETAILS:
-----------------
Resource Type: {donation.ResourceType}
Quantity: {donation.Quantity}
Description: {donation.Description}
Condition: {donation.Condition}
Delivery Method: {donation.DeliveryMethod}
Status: {donation.Status}

Thank you for your generous donation!

Gift of the Givers Foundation
{DateTime.Now.Year}
";

            // Convert to bytes (you can convert to PDF later)
            return Encoding.UTF8.GetBytes(receiptContent);
        }
    }
}