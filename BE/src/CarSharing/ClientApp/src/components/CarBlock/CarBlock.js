import React, { Component } from "react";
import './CarBlock.scss';

export default class CarBlock extends Component {
    static displayName = CarBlock.name;

    constructor(props) {
        super(props);
        this.state = { cars: [], loading: true };
    }

    componentDidMount() {
        this.populateCarData();
    }

    static renderCarsTable(cars) {
        return (
            <div className="car-block">
                {cars.map(cars =>

                    <div className={"car-content car-block " + (cars.isAvalable ? "" : "car-not-avalable")} href="">

                        <div className="car-image">
                            <img src={cars.carImage}/>
                        </div>

                        <div className="car-text">
                            <h2>{cars.title + (cars.isAvalable ? "" : " | Not avalable")}</h2>
                            <p className="car-price"> € {cars.pricePerDay}/day</p>

                            <div className="data-row">
                                <p>Transmission</p>
                                <p>{cars.transmission}</p>
                            </div>

                            <div className="data-row">
                                <p>Fuel</p>
                                <p>{cars.fuel}</p>
                            </div>

                            <div className="data-row">
                                <p>Kilometers</p>
                                <p>{cars.kilometers + (cars.isAvalable ? "" : " +")}</p>
                            </div>

                            <div className="data-row">
                                <p>Seats</p>
                                <p>{cars.seats}</p>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : CarBlock.renderCarsTable(this.state.cars);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateCarData() {
        const response = await fetch('api/GetAllCars');
        const data = await response.json();
        this.setState({ cars: data, loading: false });
    }
}