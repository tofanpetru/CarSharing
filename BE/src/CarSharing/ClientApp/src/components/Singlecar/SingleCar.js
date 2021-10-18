import React, { Component } from "react";
import './SingleCar.scss';

export default class SingleCar extends Component {
    static displayName = SingleCar.name;

    constructor(props) {
        super(props);
        this.state = { car: [], loading: true };
    }

    componentDidMount() {
        this.populateCarData();
    }

    static renderCarTable(car) {
        return (
            <div className="general-container">
                    <div>
                        <div className="single-car">
                            <div className="singlecar-container">
                                <div className="car-image">
                                    <img src={car.carImage} alt={car.title} />
                                </div>

                                <div className="car-info">
                                    <h2>{car.title}</h2>
                                    <p className="available">available</p>
                                <p className="per-day">per day<span>$ { car.pricePerDay}</span></p>
                                </div>

                            </div>
                        </div>
                        <div className="general-info">
                            <div className="general-text">
                                <h2>General Information</h2>

                                <div className="blue-line">
                                    <div></div>
                                </div>
                            </div>


                            <div className="car-details">
                                <div>
                                    <h3>Engine</h3>
                                    <p>{car.engine}</p>
                                </div>

                                <div>
                                    <h3>Kilometers</h3>
                                    <p>{car.kilometers}</p>
                                </div>

                                <div>
                                    <h3>Fuel</h3>
                                    <p>{car.fuel}</p>
                                </div>

                                <div>
                                    <h3>Seats</h3>
                                    <p>{car.seats}</p>
                                </div>

                                <div>
                                    <h3>Color</h3>
                                    <p>{car.color}</p>
                                </div>
                            </div>
                        </div>
                    </div>
             
                </div>
            );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : SingleCar.renderCarTable(this.state.car);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateCarData() {
        const response = await fetch('api/Car/Details/' + window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1));
        const data = await response.json();
        this.setState({ car: data, loading: false });
    }
}
















