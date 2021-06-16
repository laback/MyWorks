use TransportUsing

insert into UserTypes
values('admin'),('operator'),('driver')

insert into Users
values('admin','admin','Admin admin adminovich', 1),
('operator1','operator1','operator1 operator1 operator1ovich', 2),
('operator2','operator2','operator2 operator2 operator1ovich', 2),
('driver1','driver1','driver1 driver1 driver1ovich', 3),
('driver2','driver2','driver2 driver2 driver2ovich', 3),
('driver3','driver3', 'driver3 driver3 driver3ovich',3)

insert into Marks
values('BMV'),('Renault'),('Mercedes')

insert into TransportTypes
values('passenger'),('cargo'),('freight-passenger')

insert into Transports
values(1,1),(2,2),(3,3)


insert into Routes
values(4,100,'2020-01-01',1,100,0),(5,200,'2020-01-01',2,0,100),(6,300,'2020-01-01',3,50,50)