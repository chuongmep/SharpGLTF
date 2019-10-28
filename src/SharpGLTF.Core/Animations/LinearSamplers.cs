﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SharpGLTF.Animations
{
    /// <summary>
    /// Defines a <see cref="Vector3"/> curve sampler that can be sampled with STEP or LINEAR interpolations.
    /// </summary>
    struct Vector3LinearSampler : ICurveSampler<Vector3>, IConvertibleCurve<Vector3>
    {
        #region lifecycle

        public Vector3LinearSampler(IEnumerable<(float Key, Vector3 Value)> sequence, bool isLinear)
        {
            _Sequence = sequence;
            _Linear = isLinear;
        }

        #endregion

        #region data

        private readonly IEnumerable<(float Key, Vector3 Value)> _Sequence;
        private readonly Boolean _Linear;

        #endregion

        #region API

        public int MaxDegree => _Linear ? 1 : 0;

        public Vector3 GetPoint(float offset)
        {
            var segment = SamplerFactory.FindPairContainingOffset(_Sequence, offset);

            if (!_Linear) return segment.A;

            return Vector3.Lerp(segment.A, segment.B, segment.Amount);
        }

        public IReadOnlyDictionary<float, Vector3> ToStepCurve()
        {
            Guard.IsFalse(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, Vector3> ToLinearCurve()
        {
            Guard.IsTrue(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, (Vector3, Vector3, Vector3)> ToSplineCurve()
        {
            throw new NotImplementedException();
        }

        public ICurveSampler<Vector3> ToFastSampler()
        {
            var linear = _Linear;
            var split = _Sequence
                .SplitByTime()
                .Select(item => new Vector3LinearSampler(item, linear))
                .Cast<ICurveSampler<Vector3>>();

            return new FastSampler<Vector3>(split);
        }

        #endregion
    }

    /// <summary>
    /// Defines a <see cref="Quaternion"/> curve sampler that can be sampled with STEP or LINEAR interpolations.
    /// </summary>
    struct QuaternionLinearSampler : ICurveSampler<Quaternion>, IConvertibleCurve<Quaternion>
    {
        #region lifecycle

        public QuaternionLinearSampler(IEnumerable<(float, Quaternion)> sequence, bool isLinear)
        {
            _Sequence = sequence;
            _Linear = isLinear;
        }

        #endregion

        #region data

        private readonly IEnumerable<(float Key, Quaternion Value)> _Sequence;
        private readonly Boolean _Linear;

        #endregion

        #region API

        public int MaxDegree => _Linear ? 1 : 0;

        public Quaternion GetPoint(float offset)
        {
            var segment = SamplerFactory.FindPairContainingOffset(_Sequence, offset);

            if (!_Linear) return segment.A;

            return Quaternion.Slerp(segment.A, segment.B, segment.Amount);
        }

        public IReadOnlyDictionary<float, Quaternion> ToStepCurve()
        {
            Guard.IsFalse(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, Quaternion> ToLinearCurve()
        {
            Guard.IsTrue(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, (Quaternion, Quaternion, Quaternion)> ToSplineCurve()
        {
            throw new NotImplementedException();
        }

        public ICurveSampler<Quaternion> ToFastSampler()
        {
            var linear = _Linear;
            var split = _Sequence
                .SplitByTime()
                .Select(item => new QuaternionLinearSampler(item, linear))
                .Cast<ICurveSampler<Quaternion>>();

            return new FastSampler<Quaternion>(split);
        }

        #endregion
    }

    /// <summary>
    /// Defines a <see cref="Transforms.SparseWeight8"/> curve sampler that can be sampled with STEP or LINEAR interpolation.
    /// </summary>
    struct SparseLinearSampler : ICurveSampler<Transforms.SparseWeight8>, IConvertibleCurve<Transforms.SparseWeight8>
    {
        #region lifecycle

        public SparseLinearSampler(IEnumerable<(float Key, Transforms.SparseWeight8 Value)> sequence, bool isLinear)
        {
            _Sequence = sequence;
            _Linear = isLinear;
        }

        #endregion

        #region data

        private readonly IEnumerable<(float Key, Transforms.SparseWeight8 Value)> _Sequence;
        private readonly Boolean _Linear;

        #endregion

        #region API

        public int MaxDegree => _Linear ? 1 : 0;

        public Transforms.SparseWeight8 GetPoint(float offset)
        {
            var segment = SamplerFactory.FindPairContainingOffset(_Sequence, offset);

            if (!_Linear) return segment.A;

            var weights = Transforms.SparseWeight8.InterpolateLinear(segment.A, segment.B, segment.Amount);

            return weights;
        }

        public IReadOnlyDictionary<float, Transforms.SparseWeight8> ToStepCurve()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<float, Transforms.SparseWeight8> ToLinearCurve()
        {
            Guard.IsTrue(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, (Transforms.SparseWeight8, Transforms.SparseWeight8, Transforms.SparseWeight8)> ToSplineCurve()
        {
            throw new NotImplementedException();
        }

        public ICurveSampler<Transforms.SparseWeight8> ToFastSampler()
        {
            var linear = _Linear;
            var split = _Sequence
                .SplitByTime()
                .Select(item => new SparseLinearSampler(item, linear))
                .Cast<ICurveSampler<Transforms.SparseWeight8>>();

            return new FastSampler<Transforms.SparseWeight8>(split);
        }

        #endregion
    }

    /// <summary>
    /// Defines a <see cref="float"/>[] curve sampler that can be sampled with STEP or LINEAR interpolations.
    /// </summary>
    struct ArrayLinearSampler : ICurveSampler<float[]>, IConvertibleCurve<float[]>
    {
        #region lifecycle

        public ArrayLinearSampler(IEnumerable<(float, float[])> sequence, bool isLinear)
        {
            _Sequence = sequence;
            _Linear = isLinear;
        }

        #endregion

        #region data

        private readonly IEnumerable<(float Key, float[] Value)> _Sequence;
        private readonly Boolean _Linear;

        #endregion

        #region API

        public int MaxDegree => _Linear ? 1 : 0;

        public float[] GetPoint(float offset)
        {
            var segment = SamplerFactory.FindPairContainingOffset(_Sequence, offset);

            if (!_Linear) return segment.A;

            return SamplerFactory.InterpolateLinear(segment.A, segment.B, segment.Amount);
        }

        public IReadOnlyDictionary<float, float[]> ToStepCurve()
        {
            Guard.IsFalse(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, float[]> ToLinearCurve()
        {
            Guard.IsTrue(_Linear, nameof(_Linear));
            return _Sequence.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<float, (float[], float[], float[])> ToSplineCurve()
        {
            throw new NotImplementedException();
        }

        public ICurveSampler<float[]> ToFastSampler()
        {
            var linear = _Linear;
            var split = _Sequence
                .SplitByTime()
                .Select(item => new ArrayLinearSampler(item, linear))
                .Cast<ICurveSampler<float[]>>();

            return new FastSampler<float[]>(split);
        }

        #endregion
    }
}
