using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BeerPal.Web.Migrations
{
    public partial class AddedBraintree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BraintreeCustomerId",
                table: "Subscriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BraintreePlanId",
                table: "Subscriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BraintreeSubscriptionId",
                table: "Subscriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BraintreePlanId",
                table: "BillingPlans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BraintreeCustomerId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BraintreePlanId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BraintreeSubscriptionId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BraintreePlanId",
                table: "BillingPlans");
        }
    }
}
