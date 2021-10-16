import React, { Component } from "react";
import './CarBlock.scss';
import { Link } from "react-router-dom";


export default class CarBlock extends Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        };
    }

    componentDidMount() {
        fetch("api/GetAllCars")
            .then((res) => res.json())
            .then(
                (data) => {
                    this.setState({
                        data: data
                    });
                },
                (error) => {
                    console.log(error)
                }
            );
    }

    render() {
        const { data } = this.state;
        return (
                <div className="car-block">
                {data.map(car =>

                    <div className="car-content car-block" href="">

                        <div className="car-image">
                            <img src={car.carImage}/>
                        </div>

                        <div className="car-text">
                            <h2>{car.title}</h2>
                            <p className="car-price"> € {car.pricePerDay}/day</p>

                            <div className="data-row">
                                <p>Transmission</p>
                                <p>{car.transmission}</p>
                            </div>

                            <div className="data-row">
                                <p>Fuel</p>
                                <p>{car.fuel}</p>
                            </div>

                            <div className="data-row">
                                <p>Kilometers</p>
                                <p>{car.kilometers}</p>
                            </div>

                            <div className="data-row">
                                <p>Seats</p>
                                <p>{car.seats}</p>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        );
    }
}