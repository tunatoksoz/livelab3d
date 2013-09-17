import namespaces from imports.boo
switch=1

if switch==1:
	visualize
elif switch==2:
	save:
		PositionFileName="vicon.dat"
		CommandFileName="command.dat"
elif switch==3:
	play:
		PositionFileName="vicon.dat"
		CommandFileName="command.dat"
elif switch==4:
	simulate:
		Vehicles = (VehicleControllerControl( \
						Vehicle:Gpucc(Id:2,Name:"GP02",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
						Controller:SamController(), \
						Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:1,Name:"GP01",PositionalData:MotionalData(x=1,y=-2,z=0,yaw=1.57,pitch=0,roll=0)), \
							Controller:SamController(), \
							Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:3,Name:"GP03",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
							Controller:SamController(), \
							Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:4,Name:"GP04",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
							Controller:SamController(), \
							Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:5,Name:"GP05",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
							Controller:SamController(), \
							Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:6,Name:"GP06",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
							Controller:SamController(), \
							Control:GpuccControl())					
				)