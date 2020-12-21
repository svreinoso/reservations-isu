using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.API.Migrations
{
    public partial class AddRatingSp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                -- =============================================
                -- Author:		Samuel Reinoso
                -- Create date: 2020-12-12
                -- Description:	Insert new rating and update the rating in the reservation
                -- =============================================
				
                IF OBJECT_ID ( 'SP_UpdateRating', 'P' ) IS NOT NULL   
	                DROP PROCEDURE SP_UpdateRating;  
                GO 

                CREATE PROCEDURE SP_UpdateRating @stars decimal, @reservationId int, @userId varchar(20)  
                AS
                BEGIN
	                INSERT INTO Ratings VALUES (@reservationId, @stars, @userId, GETDATE(), GETDATE())
                    UPDATE Reservations SET Rating = 
                        (SELECT SUM(Star) FROM Ratings WHERE ReservationId = @reservationId) / 
                        (CONVERT(decimal, (SELECT COUNT(*) FROM Ratings WHERE ReservationId = @reservationId)))
                    WHERE Id = @reservationId
                END
                GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
