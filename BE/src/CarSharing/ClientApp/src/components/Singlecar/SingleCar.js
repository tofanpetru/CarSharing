import React, { Component } from "react";
import './SingleCar.scss';
import Audi from '../Images/Audi.png';

export default class SingleCar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        };
    }

    componentDidMount() {
        fetch("api/Car/Details/1")
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
            <div className="general-container">
                {data.map(car => 

                    <div className="single-car">
                            <div className="singlecar-container">
                                <div className="car-image">
                                    <img src={car.carImage} alt={car.title} />
                                </div>

                                <div className="car-info">
                                    <h2>{car.title}</h2>
                                    <p className="available">available</p>
                                    <p className="per-day">per day<span>$ 35*</span></p>
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
                                    <p>3d Turbo 190 KW</p>
                                </div>

                                <div>
                                    <h3>Kilometers</h3>
                                    <p>120 500</p>
                                </div>

                                <div>
                                    <h3>Fuel</h3>
                                    <p>Diesel</p>
                                </div>

                                <div>
                                    <h3>Seats</h3>
                                    <p>5</p>
                                </div>

                                <div>
                                    <h3>Color</h3>
                                    <p>Blue</p>
                                </div>
                            </div>
                        </div>
                )}
                </div>
            );
        }
}
















