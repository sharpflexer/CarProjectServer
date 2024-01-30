using CarProjectServer.BL.Commands.Cars;
using CarProjectServer.BL.Commands.Token;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.Cars;
using CarProjectServer.BL.Services.Interfaces;
using MediatR;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для взаимодействия с автомобилями в БД.
    /// </summary>
    public class CarService : ICarService
    {
        /// <summary>
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public CarService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Отправляет запрос на добавление нового автомобиля в БД через ApplicationContext.
        /// </summary>
        /// <param name="carModel">Автомобиль.</param>
        public async Task<CarModel> CreateAsync(CarModel carModel)
        {
            AddCarCommand addCar = new AddCarCommand() { Car = carModel };

            return await _mediator.Send(addCar);
        }

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="carModel">Автомобиль.</param>
        public async Task UpdateAsync(CarModel carModel)
        {
            UpdateCarCommand updateCar = new UpdateCarCommand() { Car = carModel };

            await _mediator.Send(updateCar);
        }

        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="carModel">Автомобиль.</param>
        public async Task DeleteAsync(CarModel carModel)
        {
            DeleteCarCommand deleteCar = new DeleteCarCommand() { Car = carModel };

            await _mediator.Send(deleteCar);
        }

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public async Task<IEnumerable<CarModel>> ReadAsync()
        {
            GetCarsQuery getCars = new GetCarsQuery();

            return await _mediator.Send(getCars);
        }

        /// <summary>
        /// Получает свойства автомобиля: марки, модели и цвета.
        /// </summary>
        /// <returns>Свойства автомобиля.</returns>
        public async Task<CarPropertiesModel> ReadPropertiesAsync()
        {
            GetCarPropertiesQuery getCarProperties = new GetCarPropertiesQuery();

            return await _mediator.Send(getCarProperties);
        }
    }
}
