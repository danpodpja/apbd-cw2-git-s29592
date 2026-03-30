using apbd_cw2_git_s29592.Services;
using apbd_cw2_git_s29592;

var policy = new RentalPolicy();
var equipmentService = new EquipmentService();
var userService = new UserService();
var rentalService = new RentalService(policy, equipmentService, userService);

var menu = new Demo(equipmentService, userService, rentalService);
menu.RunDemo();