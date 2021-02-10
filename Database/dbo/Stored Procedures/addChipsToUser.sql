CREATE PROCEDURE addChipsToUser

    @UserId int,
    @ChipsAmount money
AS
BEGIN



    Update Money
    set Chips = (SELECT Chips from Money where Id = @UserId) + @ChipsAmount
    where Money.Id = @UserId;

END